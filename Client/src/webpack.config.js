import { resolve as _resolve } from "path";
import HtmlWebpackPlugin from "html-webpack-plugin";
import { CleanWebpackPlugin } from "clean-webpack-plugin";
import MiniCssExtractPlugin, { loader } from "mini-css-extract-plugin";

export const entry = "./src/index.ts";
export const output = {
    path: _resolve(__dirname, "wwwroot"),
    filename: "[name].[chunkhash].js",
    publicPath: "/",
};
export const resolve = {
    extensions: [".js", ".ts"],
};
export const module = {
    rules: [
        {
            test: /\.ts$/,
            use: "ts-loader",
        },
        {
            test: /\.css$/,
            use: [loader, "css-loader"],
        },
    ],
};
export const plugins = [
    new CleanWebpackPlugin(),
    new HtmlWebpackPlugin({
        template: "./src/index.html",
    }),
    new MiniCssExtractPlugin({
        filename: "css/[name].[chunkhash].css",
    }),
];