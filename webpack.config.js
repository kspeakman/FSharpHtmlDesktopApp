var path = require('path');
var webpack = require('webpack');
var HtmlWebpackPlugin = require('html-webpack-plugin');
var CopyWebpackPlugin = require('copy-webpack-plugin');
var MiniCssExtractPlugin = require('mini-css-extract-plugin');


function resolve(filePath) {
    return path.isAbsolute(filePath) ? filePath : path.join(__dirname, filePath);
}

// The HtmlWebpackPlugin allows us to use a template for the index.html page
// and automatically injects <script> or <link> tags for generated bundles.
var htmlPlugin =
    new HtmlWebpackPlugin({
        filename: 'index.html',
        template: resolve('./src/UI/index.html')
    });

// Copies static assets to output directory
var copyPlugin =
    new CopyWebpackPlugin({
        patterns: [{
            from: resolve('./src/UI/public')
        }]
    });

// CSS bundling
var cssPlugin =
    new MiniCssExtractPlugin({
        filename: 'style.[name].[contenthash].css'
    });

// HMR
var hmrPlugin =
    new webpack.HotModuleReplacementPlugin();


// Configuration for webpack-dev-server
var devServer = {
    static: { directory: resolve('./src/UI/public') },
    host: 'localhost',
    port: 8080,
    hot: true
};

// If we're running the webpack-dev-server, assume we're in development mode
var isProduction = !process.argv.find(v => v.indexOf('webpack-dev-server') !== -1);
var environment = isProduction ? 'production' : 'development';
process.env.NODE_ENV = environment;
console.log('Bundling for ' + environment + '...');

module.exports = {
    entry: { app: resolve('./src/UI/Startup.fs.js') },
    output: {
        path: resolve('./deploy/public'),
        filename: isProduction ? '[name].[contenthash].js' : '[name].js'
    },
    devtool: isProduction ? 'source-map' : 'eval-source-map',
    resolve: { symlinks: false }, // See https://github.com/fable-compiler/Fable/issues/1490
    mode: isProduction ? 'production' : 'development',
    plugins: isProduction ? [cssPlugin, htmlPlugin, copyPlugin] : [htmlPlugin, hmrPlugin],
    optimization: { splitChunks: { chunks: 'all' } },
    devServer: devServer,
    module: {
        rules: [
            {
                test: /\.(sass|scss|css)$/,
                use: [
                    isProduction
                        ? MiniCssExtractPlugin.loader
                        : 'style-loader',
                    'css-loader',
                    {
                        loader: 'sass-loader',
                        options: { implementation: require('sass') }
                    }
                ]
            },
            {
                test: /\.(png|jpg|jpeg|gif|svg|woff|woff2|ttf|eot)(\?.*)?$/,
                use: ['file-loader']
            }
        ]
    }
};