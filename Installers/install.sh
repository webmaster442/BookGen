#!/bin/bash

#check for admin rights
if [ "$EUID" -ne 0 ]
  then echo "Please run as root"
  exit
fi

mkdir -p /opt/bookgen
cp bin/* /opt/bookgen
ln -s /opt/bookgen/BookGen /usr/bin/bookgen

echo "BookGen installed successfully!"