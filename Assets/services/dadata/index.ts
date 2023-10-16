import { SuggestionsListContract } from "./contracts.ts";

const BASE_URL = "/api/dadata";

export const suggestAddressAsync = async (query: string) => {
    if (query.length === 0) return [];

    const urlQuery = new URLSearchParams({
        query,
    });

    const response = await fetch(BASE_URL + `/suggest-address?${urlQuery}`).then((r) => r.json());
    return SuggestionsListContract.check(response);
};

export const suggestInitialsAsync = async (query: string) => {
    if (query.length === 0) return [];

    const urlQuery = new URLSearchParams({
        query,
    });

    const response = await fetch(BASE_URL + `/suggest-initials?${urlQuery}`).then((r) => r.json());
    return SuggestionsListContract.check(response);
};

export const suggestCompanyAsync = async (query: string) => {
    if (query.length === 0) return [];

    const urlQuery = new URLSearchParams({
        query,
    });

    const response = await fetch(BASE_URL + `/suggest-company?${urlQuery}`).then((r) => r.json());
    return SuggestionsListContract.check(response);
};

export * from "./contracts.ts";
