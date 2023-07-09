#!/bin/bash
URL_FILE="https://github.com/webmaster442/BookGen/blob/master/.github/linux-latest.txt"
TARGET_DIR="/opt/bookgen"
DOWNLOAD_URL=$(curl -s "$URL_FILE")
curl -o bookgen.tar.gz "$DOWNLOAD_URL"
tar -xf bookgen.tar.gz -C "$TARGET_DIR"
rm bookgen.tar.gz

SCRIPT_PATH="/usr/bin/bookgen"
COMMAND="/opt/bookgen/BookGen"
cat <<EOF > "$SCRIPT_PATH"
#!/bin/bash
$COMMAND "\$@"
EOF

chmod +x "$SCRIPT_PATH"