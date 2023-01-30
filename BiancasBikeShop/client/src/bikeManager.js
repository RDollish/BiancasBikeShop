const apiUrl = '/api/bike';

export const getBikes = async () => {
    const response = await fetch(apiUrl);
    return await response.json();
};

export const getBikeById = async (id) => {
    const response = await fetch(`${apiUrl}/${id}`);
    return await response.json();
};

export const getBikesInShopCount = async () => {
    const response = await fetch(`${apiUrl}/GetBikesInShopCount`);
    return await response.json();
};
