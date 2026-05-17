import { MutationCache, QueryCache, QueryClient } from "@tanstack/react-query";

export const queryClient = new QueryClient({
    queryCache: new QueryCache({
        onError: (error: any) =>{
            console.error("Query error:", error);
        }
    }),
    mutationCache: new MutationCache({
        onError: (error: any) =>{
            console.error("Mutation error:", error);
        }
    }),
    defaultOptions: {
        queries: {
            staleTime: 5 * 60 * 1000, // 5 minutes
            retry: 1,
            refetchOnWindowFocus: false,
        }
    }
});