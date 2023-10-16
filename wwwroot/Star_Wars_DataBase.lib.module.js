window.__blazor__ = new Promise(() => {

});

export function afterStarted(blazor) {
    window.__blazor__ = Promise.resolve(blazor);
}
