import axios, { AxiosError, type InternalAxiosRequestConfig } from "axios";
import { clearTokens, getToken, setTokens } from "../lib/storage";
import { toast } from "sonner";
const apiUrl = import.meta.env.VITE_API_URL;

const apiClient = axios.create({
    baseURL: apiUrl,
    headers: {
        "Content-Type": "application/json"
    }
});

// Request interceptor
apiClient.interceptors.request.use(
    (config: InternalAxiosRequestConfig) => {
        const { accessToken } = getToken();
        if (accessToken && config.headers) {
            config.headers.Authorization = `Bearer ${accessToken}`;
        }
        return config;
    },
    (error) => Promise.reject(error)
);

// Response interceptor
let isRefreshing = false;
let failedQueue: Array<{
    resolve: (token: string) => void;
    reject: (error: any) => void;
}> = [];

const processQueue = (error: AxiosError | null, token: string | null) => {
    failedQueue.forEach(({ resolve, reject }) => {
        if (error) {
            reject(error);
        } else if (token) {
            resolve(token);
        }
    });
    failedQueue = [];
}

apiClient.interceptors.response.use(
    (response) => response.data,
    async (error: AxiosError) => {
        const originalRequest = error.config as InternalAxiosRequestConfig & { _retry?: boolean };
        if (error.response?.status === 401 && !originalRequest._retry) {
            if (isRefreshing) {
                return new Promise((resolve, reject) => {
                    failedQueue.push({
                        resolve: (token: string) => {
                            originalRequest.headers.Authorization = `Bearer ${token}`;
                            resolve(apiClient(originalRequest));
                        },
                        reject
                    });
                });
            }
            originalRequest._retry = true;
            isRefreshing = true;

            try {
                const { refreshToken } = getToken();
                const response = await axios.post(`${apiUrl}/auth/refresh`, { refreshToken });
                const { accessToken: newAccess, refreshToken: newRefresh } = response.data;

                setTokens({ accessToken: newAccess, refreshToken: newRefresh });
                processQueue(null, newAccess);
                originalRequest.headers.Authorization = `Bearer ${newAccess}`;
                return apiClient(originalRequest);
            } catch (refreshError) {
                processQueue(refreshError as AxiosError, null);
                clearTokens();
                window.location.href = "/login";
                return Promise.reject(refreshError);
            }
            finally {
                isRefreshing = false;
            }
        }

        handleHttpError(error);
        return Promise.reject(error);
    }
)

function handleHttpError(error: AxiosError) {
    if (error.response) {
        const status = error.response.status;
        const message = (error.response.data as any)?.message || "An error occurred";
        toast.error(`Error ${status}: ${message}`);
    } else if (error.request) {
        toast.error("No response from server. Please check your connection.");
    }
}

export default apiClient;