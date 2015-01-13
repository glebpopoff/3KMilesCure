module.exports = function (grunt) {

    // Project configuration.
    grunt.initConfig({

        pkg: grunt.file.readJSON("package.json"),

        less: {
            options: {
                compress: true,
                yuicompress: true,
                optimization: 2,
                sourceMap: true,
                sourceMapFilename: 'ui/css/main-styles.css.map', // where file is generated and located
                sourceMapURL: 'main-styles.css.map', // the complete url and filename put in the compiled css file
                outputSourceFiles: true
            },

            styles: {
                options: {
					sourceMapFilename: 'ui/css/main-styles.css.map' // where file is generated and located
					, sourceMapURL: 'main-styles.css.map' // the complete url and filename put in the compiled css file
				},
				files: {
                    "ui/css/main-styles.css": "ui/less/main-styles.less"
                }
            }
        },

        jshint: {
            options: {
                browser: true,
                devel: true,
                noempty: true,
                plusplus: false,
                supernew: true,
                unused: false,
                evil: false,
                bitwise:true,
                freeze:false,
                laxcomma: true,
                nomen: true,
                debug: true,
                expr: true,
                newcap: true,
                validthis: true,

                globals: {
                    log: true,
                    define: true
                }
            },
            dev: {
                src: ["ui/scripts/**/*.js", "!ui/scripts/libs/**/*.js", "!ui/scripts/vendor/**/*.js"]
            }
        },

        jslint: {
            dev: {
                src: ["ui/scripts/**/*.js", "!ui/scripts/libs/**/*.js", "!ui/scripts/vendor/**/*.js"],
                options: {
                    edition: "latest",
                    errorsOnly: false
                },
                directives: {
                    browser: true,
                    devel: true,
                    evil: true,
                    nomen: true,
                    plusplus: true,
                    regexp: true,
                    unparam: true,
                    vars: true,
                    white: true,
                    globals: {
                        require: true,
                        define: true,
                        log: true,
                        Modernizr: true,
                        addthis: true,
                        LimelightPlayer: true,
                        LimelightPlayerUtil: true
                    }
                }
            }
        },

        imagemin: {
            dynamic: {                         // Another target
                files: [{
                    expand: true,                       // Enable dynamic expansion
                    cwd: 'ui/images',                   // Src matches are relative to this path
                    src: ['**/*.{jpg,png,gif,svg}'],    // Actual patterns to match
                    dest: 'ui/images'              // Destination path prefix
                }]
            }
        },

        clean: ["dist"],

        watch: {
            options: {
                livereload: true
            },
            css: {
                files: ["ui/less/**/*.less", "ui/css/**/*.css", "!ui/css/main-styles.css", "!ui/css/main-styles.css.map"],
                tasks: ["less:styles"],
                options: {}
            },
            js: {
                files: ['ui/scripts/**/*.js', '!ui/scripts/libs/**/*.js', '!ui/scripts/vendor/**/*.js'],
                tasks: ["jshint:dev", "jslint:dev"]
            }
        }

    });

    require('matchdep').filterDev('grunt-*').forEach(grunt.loadNpmTasks);

    grunt.registerTask("default", ["less", "jshint", "jslint"]);

    grunt.registerTask("server", ["less", "imagemin", "jshint:dev", "jslint:dev"]);

};

