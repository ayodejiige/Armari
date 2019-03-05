from flask import Flask
from flask_sqlalchemy import SQLAlchemy
import enum
# import pyodbc
from sqlalchemy import create_engine
import urllib

# params = urllib.parse.quote_plus(r'Driver={ODBC Driver 13 for SQL Server};Server=tcp:armari.database.windows.net,1433;Database=Wardrobe;Uid=armari@armari;Pwd=Armari2019;Encrypt=yes;TrustServerCertificate=no;Connection Timeout=30;')
# conn_str = 'mssql+pyodbc:///?odbc_connect={}'.format(params)

# cnxn = pyodbc.connect("Driver={ODBC Driver 13 for SQL Server};Server=tcp:armari.database.windows.net,1433;Database=Wardrobe;Uid=armari@armari;Pwd=WAterloo2019;Encrypt=yes;TrustServerCertificate=no;Connection Timeout=30;")
# cursor = cnxn.cursor()
# cursor.execute("select @@VERSION")
# row = cursor.fetchone()
# if row:
#     print 

app = Flask(__name__)
app.config['SQLALCHEMY_DATABASE_URI'] = 'sqlite:///./test.db'
app.config['SQLALCHEMY_TRACK_MODIFICATIONS'] = True
db = SQLAlchemy(app)

class CompartmentStates(enum.Enum):
    DANGLING=0
    AVAILABLE=1
    FULL=2

class CompartmentPosition(object):
    x = None
    y = None

class CompartmentType(enum.Enum):
    FOLDABLE=0
    HANGER=1
    FOOTWEAR=2

class User(db.Model):
    id = db.Column(db.Integer, primary_key=True)
    username = db.Column(db.String(80), unique=True, nullable=False)
    email = db.Column(db.String(120), unique=True, nullable=False)
    wardrobe = db.relationship('Wardrobe', backref=db.backref('user', lazy=True, uselist=False))

    def __init__(self, username, email):
        self.username = username
        self.email = email
        self.wardrobe.append(Wardrobe())

    def __repr__(self):
        wardrobe_str = str(self.wardrobe[0])
        ret = '<username %s> \n' % self.username
        ret += wardrobe_str

        return ret

    def get_wardrobe(self):
        return self.wardrobe[0]

    @classmethod
    def get_user(cls, user_id):
        user = cls.query.filter_by(id=user_id).one()
        return user

class Wardrobe(db.Model):
    id = db.Column(db.Integer, primary_key=True)
    user_id = db.Column(db.Integer, db.ForeignKey('user.id'),
        nullable=False)
    compartments = db.relationship('Compartment', backref='wardrobe', lazy=True)
    

    def __init__(self):
        default_position = CompartmentPosition();
        default_position.x = -1
        default_position.y = -1
        compartment = Compartment(state=CompartmentStates.DANGLING, position=default_position)
        self.compartments.append(compartment)

    def __repr__(self):
        comp_str = ''
        for compartment in self.compartments:
            comp_str += str(compartment)
            comp_str += '\n'
        ret = '\t<wardrobe id %d> \n' %self.id
        ret += comp_str

        return ret

    def get_dangling_compartment(self):
        compartment = Compartment.query.filter_by(
            state=CompartmentStates.DANGLING, wardrobe_id=self.id).first()

        return compartment

    def get_free_compartment(self):
        compartment = Compartment.query.filter_by(
            state=CompartmentStates.AVAILABLE, wardrobe_id=self.id).first()

        return compartment

    def add_compartment(self, position):
        compartment = Compartment(state=CompartmentStates.AVAILABLE, position=position)
        self.compartments.append(compartment)

        return compartment
    
    def get_cloth_count(self):
        count = 0
        for compartment in self.compartments:
            if compartment.state == CompartmentStates.DANGLING:
                continue
            for item in compartment.clothitems:
                count +=1
        
        return count

class Compartment(db.Model):
    id = db.Column(db.Integer, primary_key=True)
    state = db.Column(db.Enum(CompartmentStates))
    position = db.Column(db.Enum(CompartmentStates))
    type = db.Column(db.Enum(CompartmentType))
    wardrobe_id = db.Column(db.Integer, db.ForeignKey('wardrobe.id'),
        nullable=False)
    clothitems = db.relationship('ClothingItem', backref='compartment', lazy=True)

    def __init__(self, state, position):
        self.state = state
        # self.position = position

    def __repr__(self):
        cl_str = ''
        for cloth in self.clothitems:
            cl_str += str(cloth)
            cl_str += '\n'
        ret = '\t\t<id %d> <State %s> <Pos (%d %d)>\n' % (self.id, self.state, 0, 0)
        ret += cl_str

        return ret
    
    def add_cloth(self, cloth):
        self.clothitems.append(cloth)
        # logic to update state here
    
    def get_cloth(self, id):
        self.clothitems.query.filter_by(
            id=id).first()
    
    def remove_cloth(self, cloth):
        self.clothitems.remove(cloth)
    

class ClothingItem(db.Model):
    id = db.Column(db.Integer, primary_key=True)
    type = db.Column(db.String(80), nullable=False)
    compartment_id = db.Column(db.Integer, db.ForeignKey('compartment.id'),
        nullable=False)
    
    def __repr__(self):
        ret = '\t\t\t<id %d> <type %s>' %(self.id, self.type)
        return ret
    
    def get_compartment(self):
        compartment = Compartment.query.filter_by(
            id=self.compartment_id).first()

        return compartment

class DBManager(object):
    def __init__(self, user_id):
        self._user_id = user_id
        self._user = User.get_user(self._user_id)
        self._wardrobe = self._user.get_wardrobe()
    
    @classmethod
    def new_user(email, name):
        user = User(username=name, email=email)
        db.session.add(user)
        db.session.commit()

        return user.id
    
    def print_user(self):
        print(self._user)
        print(self.get_wardrobe_state())

    def cloth_count(self):
        return self._wardrobe.get_cloth_count()

    def add_compartment(self, position):
        compartment = self._wardrobe.add_compartment(position)

        return compartment

    def get_compartment(self):
        compartment = self._wardrobe.get_free_compartment()

        return compartment

    def new_cloth(self, compartment, cloth_type):
        cloth = ClothingItem(type=cloth_type)
        compartment.add_cloth(cloth)
        db.session.commit()

        return cloth.id

    def return_cloth(self, cloth_id):
        pass

    def retrieve_cloth(self, cloth_id):
        cloth = ClothingItem.query.filter_by(id=cloth_id).first()

        return cloth
    
    def retrieved_cloth(self, cloth_id):
        cloth = ClothingItem.query.filter_by(id=cloth_id).first()
        compartment = cloth.get_compartment()

        dangling_compartment = self._wardrobe.get_dangling_compartment()
        dangling_compartment.add_cloth(cloth)
        db.session.commit()
    
    def return_cloth(self, compartment, cloth_id):
        cloth = ClothingItem.query.filter_by(id=cloth_id).first()
        compartment.add_cloth(cloth)
        db.session.commit()

        return cloth
    
    def remove_cloth(self, cloth_id):
        cloth = ClothingItem.query.filter_by(id=cloth_id).first()
        db.session.delete(cloth)
        db.session.commit()

    def get_cloth_by_type(self, cloth_id):
        cloth = ClothingItem.query.filter_by(id=cloth_id).first()

    def get_wardrobe_state(self):
        compartments = self._wardrobe.compartments
        res = {}
        for compartment in compartments:
            clothes = compartment.clothitems
            res[compartment.id] = {}
            for c in clothes:
                c_dict = {}
                res[compartment.id][c.id] = {}
                res[compartment.id][c.id]["type"] = c.type
                
        return res
        


def init():
    default_position = CompartmentPosition();
    default_position.x = 0
    default_position.y = 0
    db.create_all()
    user = User(username='deji', email='admin@email2')
    wardrobe = user.get_wardrobe()
    compartment = wardrobe.add_compartment(default_position)
    default_position.y = 1
    compartment = wardrobe.add_compartment(default_position)
    default_position.x = 1
    compartment = wardrobe.add_compartment(default_position)
    db.session.add(user)
    db.session.commit()

def main():
    init()
    manager = DBManager(1)
    manager.print_user()


if __name__ == "__main__":
    main()