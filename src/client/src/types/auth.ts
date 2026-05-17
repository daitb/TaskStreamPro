export interface LoginResponse{
    accessToken: string;
    refreshToken: string;
    user: UserProfile;
}

export interface LoginRequest{
    email: string;
    password: string;
}

export interface UserProfile{
    id: number;
    email: string;
    fullName: string;
    avatarUrl: string;
    systemRole: string;
}

export interface ApiResponse<T = unknown> {
    status: "success" | "error";
    message: string;
    data: T;
    statusCode: number;
}