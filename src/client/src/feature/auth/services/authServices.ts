import { useMutation, useQuery } from "@tanstack/react-query"
import type { LoginRequest } from "../../../types/auth"
import { authApi } from "../../../api/endpoints/auth.api"
import { clearTokens, setTokens } from "../../../lib/storage"

export const useLogin = () => {
    return useMutation({
        mutationFn: (data: LoginRequest) => authApi.login(data),
        onSuccess: (response) => {
            setTokens({
                accessToken: response.data.accessToken,
                refreshToken: response.data.refreshToken
            })
        } 
    })
}

export const useProfile = () => {
    return useQuery({
        queryKey: ["profile"],
        queryFn: () => authApi.getProfile()
    })
}

export const useLogout = () => {
    return useMutation({
        mutationFn: () => authApi.logout(),
        onSettled: () => clearTokens()
    })
}