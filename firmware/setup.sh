# Script to install required packages
# Setup ssh
# Setup network
# Setup keyboard
# Setup i2c
# Connect to eduroam?

echo "Install mqtt broker"
sudo apt-get install -y software-properties-common # to get apt-add-repo
sudo apt-add-repository ppa:mosquitto-dev/mosquitto-ppa
sudo apt-get update
sudo apt-get install mosquitto

echo "Install mqtt command line tools"
sudo apt-get install mosquitto-clients

echo "Install mqtt client or python"
pip install paho-mqtt --user

# I2C tool
sudo apt-get install -y python-smbus
sudo apt-get install -y i2c-tools

# Install Adafruit GPIO
sudo apt-get install build-essential python-pip python-dev python-smbus git
git clone https://github.com/adafruit/Adafruit_Python_GPIO.git
cd Adafruit_Python_GPIO
sudo python setup.py install
cd ..
rm -rfv Adafruit_Python_GPIO