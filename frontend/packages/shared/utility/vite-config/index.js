import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';

export const sharedConfig = defineConfig({
    plugins: [react()],
    resolve: {
        alias: {
            '@': '/src',
        },
    },
    build: {
        sourcemap: true,
    },
});

export default sharedConfig;