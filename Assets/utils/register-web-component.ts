export const registerWebComponent = (
    tag: string,
    CustomElementClass: CustomElementConstructor,
) => {
    try {
        customElements.define(tag, CustomElementClass);
    } catch (e) {
        console.warn(`Redefining tag: ${tag}`, e);
    }

    if (!customElements.get(tag)) {
        try {
            customElements.define(tag, CustomElementClass);
        } catch (e) {
            console.error(
                `Failed to register element in HMR environment: ${tag}`,
                e,
            );
        }
    }
};
