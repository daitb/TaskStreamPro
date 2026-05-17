import type { ApiResponse, LoginRequest, LoginResponse, UserProfile } from "../../types/auth";
import apiClient from "../apiClient";

export const authApi = {
    login : (data : LoginRequest) : Promise<ApiResponse<LoginResponse>> => 
        apiClient.post("auth/login", data),

    refreshToken : (refreshToken : string) : Promise<ApiResponse<LoginResponse>> =>
        apiClient.post("auth/refresh", { refreshToken }),
    
    logout : () => 
        apiClient.post("auth/logout"),

    getProfile: () : Promise<ApiResponse<UserProfile>> => 
        apiClient.get("auth/profile")
}