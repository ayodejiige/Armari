#!/bin/bash
USER=pi
HOST="10.42.0.10"
DEST="/home/pi/firmware"
DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
cd $DIR
scp -r . $USER@$HOST:$DEST

ssh $USER@$HOST << "EOF"
sudo echo "alias led='python /home/pi/firmware/led.py'" >> ~/.bashrc
source ~/.bashrc
EOF