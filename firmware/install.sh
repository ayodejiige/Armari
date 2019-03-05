#!/bin/bash
USER=pi
HOST="192.168.1.15"
DEST="/home/pi/firmware"
DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
cd $DIR
scp -r . $USER@$HOST:$DEST

ssh $USER@$HOST << "EOF"
sudo echo "alias led='python3 /home/pi/firmware/led.py'" >> ~/.bashrc
sudo echo "alias manager='python3 /home/pi/firmware/manager.py'" >> ~/.bashrc
source ~/.bashrc
EOF