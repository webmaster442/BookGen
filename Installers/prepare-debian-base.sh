#!/bin/bash

#check for admin rights
if [ "$EUID" -ne 0 ]
  then echo "Please run as root"
  exit
fi

apt update
apt dist-upgrade -y
apt install -y curl wget mc

wget https://packages.microsoft.com/config/debian/13/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
rm packages-microsoft-prod.deb

apt update
apt install -y dotnet-sdk-10.0
apt autoremove --purge
apt clean