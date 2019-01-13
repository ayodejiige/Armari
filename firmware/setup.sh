# Script to install required packages

echo "Install mqtt broker"
sudo apt-add-repository ppa:mosquitto-dev/mosquitto-ppa
sudo apt-get update
sudo apt-get install mosquitto

echo "Install mqtt command line tools"
sudo apt-get install mosquitto-clients

echo "Install mqtt client or python"
pip install paho-mqtt --user