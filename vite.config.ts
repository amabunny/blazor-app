import { UserConfig, defineConfig } from "vite";
import { spawn } from "child_process";
import { fileURLToPath, URL } from "url";
import fs from "fs";
import path from "path";
import react from "@vitejs/plugin-react";

import appsettings from "./appsettings.json";
import appsettingsDev from "./appsettings.Development.json";

// Get base folder for certificates.
const baseFolder =
    process.env.APPDATA !== undefined && process.env.APPDATA !== ""
        ? `${process.env.APPDATA}/ASP.NET/https`
        : `${process.env.HOME}/.aspnet/https`;

// Generate the certificate name using the NPM package name
const certificateName = process.env.npm_package_name;

// Define certificate filepath
const certFilePath = path.join(baseFolder, `${certificateName}.pem`);
// Define key filepath
const keyFilePath = path.join(baseFolder, `${certificateName}.key`);

// Pattern for CSS files
const cssPattern = /\.css$/;
// Pattern for image files
const imagePattern = /\.(png|jpe?g|gif|svg|webp|avif)$/;

// Export Vite configuration
export default defineConfig(async () => {
    // Ensure the certificate and key exist
    if (!fs.existsSync(certFilePath) || !fs.existsSync(keyFilePath)) {
        // Wait for the certificate to be generated
        await new Promise<void>((resolve) => {
            spawn("dotnet", ["dev-certs", "https", "--export-path", certFilePath, "--format", "Pem", "--no-password"], {
                stdio: "inherit",
            }).on("exit", (code) => {
                resolve();
                if (code) {
                    process.exit(code);
                }
            });
        });
    }

    // Define Vite configuration
    const config: UserConfig = {
        plugins: [
            react({
                babel: {
                    plugins: [["effector/babel-plugin", { addLoc: process.env.NODE_ENV === "development" }]],
                },
            }),
        ],
        appType: "custom",
        root: "Assets",
        publicDir: "public",
        resolve: {
            alias: [
                {
                    find: "@",
                    replacement: fileURLToPath(new URL("./Assets", import.meta.url)),
                },
            ],
        },
        build: {
            manifest: appsettings.Vite.Manifest,
            emptyOutDir: false,
            outDir: "../wwwroot",
            assetsDir: "",
            rollupOptions: {
                input: "Assets/main.ts",
                output: {
                    // Save entry files to the appropriate folder
                    entryFileNames: "assets/js/[name].[hash].js",
                    // Save chunk files to the js folder
                    chunkFileNames: "assets/js/[name]-chunk.js",
                    // Save asset files to the appropriate folder
                    assetFileNames: (info) => {
                        if (info.name) {
                            // If the file is a CSS file, save it to the css folder
                            if (cssPattern.test(info.name)) {
                                return "assets/css/[name][extname]";
                            }
                            // If the file is an image file, save it to the images folder
                            if (imagePattern.test(info.name)) {
                                return "assets/images/[name][extname]";
                            }

                            // If the file is any other type of file, save it to the assets folder
                            return "assets/misc/[name][extname]";
                        } else {
                            // If the file name is not specified, save it to the output directory
                            return "[name][extname]";
                        }
                    },
                },
            },
        },
        server: {
            port: appsettingsDev.Vite.Server.Port,
            strictPort: true,
            https: appsettingsDev.Vite.Server.UseHttps
                ? {
                      cert: certFilePath,
                      key: keyFilePath,
                  }
                : undefined,
            hmr: {
                clientPort: appsettingsDev.Vite.Server.Port,
            },
            watch: {
                usePolling: true,
            },
        },
        optimizeDeps: {
            include: [],
        },
    };

    return config;
});
