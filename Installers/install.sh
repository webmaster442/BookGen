#!/bin/sh

#check for admin rights
if [ "$EUID" -ne 0 ]
  then echo "Please run as root"
  exit
fi

cp bin/* /opt/bookgen
ln -s /opt/bookgen/bookgen /usr/local/bin/bookgen

echo "BookGen installed successfully!"
