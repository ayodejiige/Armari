from database import *

# global test variables
test_cloth_id = None
class TestClass(object):
    _manager = DBManager(1)
    _test_cloth_id = None

    def test_newitem(self):
        global test_cloth_id
        compartment = self._manager.get_compartment()
        count = self._manager.cloth_count()
        test_cloth_id = self._manager.new_cloth(compartment, "test")
        count_new = self._manager.cloth_count()

        assert count_new == count+1

        count = self._manager.cloth_count()
        test_cloth_id = self._manager.new_cloth(compartment, "test")
        count_new = self._manager.cloth_count()
        assert count_new == count+1

        self._manager.print_user()

    def test_retrieveitem(self):
        count = self._manager.cloth_count()
        cloth = self._manager.retrieve_cloth(test_cloth_id)
        assert cloth.type == "test"

        self._manager.retrieved_cloth(test_cloth_id)
        compartment = Compartment.query.filter_by(id=cloth.compartment_id).first()
        assert compartment.state == CompartmentStates.DANGLING

        count_new = self._manager.cloth_count()
        assert count_new == count-1

        self._manager.print_user()
    
    def test_returnitem(self):
        count = self._manager.cloth_count()
        compartment = self._manager.get_compartment()
        cloth =  self._manager.return_cloth(compartment, test_cloth_id)
        new_comparment = cloth.compartment

        assert compartment == new_comparment

        count_new = self._manager.cloth_count()
        assert count_new == count+1

        self._manager.print_user()

    def test_cleanup(self):
        clothes = ClothingItem.query.filter_by(type="test").all()

        for cloth in clothes:
            self._manager.remove_cloth(cloth.id)

        self._manager.print_user()