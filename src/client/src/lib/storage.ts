const TOKEN_KEY = "auth_token";

interface Tokens{
    accessToken: string;
    refreshToken: string;
}

export const getToken = () : Partial<Tokens> => {
    try{
        const data = localStorage.getItem(TOKEN_KEY);
        return data ? JSON.parse(data) : {};
    }
    catch{
        return {};
    }
}

export const setTokens = (token: Tokens) => {
    localStorage.setItem(TOKEN_KEY, JSON.stringify(token));
}

export const clearTokens = () => {
    localStorage.removeItem(TOKEN_KEY);
}