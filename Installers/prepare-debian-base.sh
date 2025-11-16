#!/bin/bash

#check for admin rights
if [ "$EUID" -ne 0 ]
  then echo "Please run as root"
  exit
fi

apt update
apt dist-upgrade -y
apt install -y curl wget mc
apt autoremove --purge
apt clean