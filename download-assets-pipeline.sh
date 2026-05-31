#!/bin/sh
mkdir -p "./Assets/tools"
cd "./Assets/tools"
curl -L -O "https://github.com/erweixin/RaTeX/releases/download/v0.1.10/ratex-cli-v0.1.10-aarch64-unknown-linux-musl.tar.gz"
tar -xzf ./ratex-cli-v0.1.10-aarch64-unknown-linux-musl.tar.gz -C .
mv ./ratex-cli-v0.1.10-aarch64-unknown-linux-musl/render-svg ./ratex-svg