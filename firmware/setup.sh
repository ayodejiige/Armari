# Script to install required packages
# Setup ssh
# Setup network
# Setup keyboard
# Setup i2c
# Connect to eduroam?

sudo apt-get update

echo "======================================="
echo "Update Python pip"
echo "======================================="
sudo apt install -y python3-pip

echo "======================================="
echo "Install mqtt broker"
echo "======================================="
sudo apt-get install -y software-properties-common # to get apt-add-repo
sudo apt-add-repository ppa:mosquitto-dev/mosquitto-ppa
sudo apt-get update
sudo apt-get install -y mosquitto

echo "======================================="
echo "Install mqtt command line tools"
echo "======================================="
sudo apt-get install -y mosquitto-clients

echo "======================================="
echo "Install mqtt client or python"
echo "======================================="
pip3 install paho-mqtt --user

# I2C tool
echo "======================================="
echo "Install i2c tools"
echo "======================================="
sudo apt-get install -y python-smbus
sudo apt-get install -y i2c-tools

# Server Packages
echo "======================================="
echo "Install database and server libs"
echo "======================================="
pip3 install flask --user
pip3 install sqlalchemy --user
pip3 install flask-sqlalchemy --user

# Install Adafruit GPIO
echo "======================================="
echo "Install Adafruit GPIO"
echo "======================================="
sudo apt-get install -y build-essential python-pip python-dev python-smbus git
git clone https://github.com/adafruit/Adafruit_Python_GPIO.git
cd Adafruit_Python_GPIO
sudo python3 setup.py install --user
cd ..
sudo rm -rfv Adafruit_Python_GPIO