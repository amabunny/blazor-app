/* eslint-disable @typescript-eslint/no-explicit-any */
/* eslint-disable @typescript-eslint/ban-ts-comment */

let _blazor: IBlazor | null = null;

interface IBlazor {
    registerCustomEventType: (
        name: string,
        options: {
            createEventArgs: (event: any) => any;
        },
    ) => void;
}

interface IRegister {
    registerEvent: (customEvent: ICreatedCustomEvent<any, any>) => void;
}

export const registerCustomEvents = (fn: (register: IRegister) => void) => {
    // @ts-ignore
    const task: Promise<IBlazor> | object = window.__blazor__;

    try {
        if (task instanceof Promise) {
            task.then((blazor) => {
                // @ts-ignore
                window.__blazor__ = null;
                _blazor = blazor;

                fn({
                    registerEvent: (customEvent) => {
                        blazor.registerCustomEventType(customEvent.name, {
                            createEventArgs: (event: CustomEvent) => event.detail,
                        });
                    },
                });
            });
        } else {
            if (_blazor) {
                fn({
                    registerEvent: (customEvent) => {
                        _blazor?.registerCustomEventType(customEvent.name, {
                            createEventArgs: (event: CustomEvent) => event.detail,
                        });
                    },
                });
            } else {
                console.warn("Empty blazor instance");
            }
        }
    } catch {}
};

interface ICreatedCustomEvent<T, D> {
    name: T;
    trigger: (node: Element | null, detail: D) => void;
}

export const createCustomEvent = <D extends object, T extends string>(type: T): ICreatedCustomEvent<T, D> => ({
    name: type,
    trigger: (node: Element | null, detail: D) => {
        const event = new CustomEvent<D>(type, {
            bubbles: true,
            composed: true,
            detail,
        });

        node?.dispatchEvent(event);
    },
});
