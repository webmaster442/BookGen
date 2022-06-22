function sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}

async function main() {
    let i = 0;
    for (; i < 10; i++) {
        console.log(i);
        await sleep(200);
    }
}

main();