(function () {
    'use strict';

    var fileLists = {
        html: ['*.html', 'content/**/*.html', 'views/**/*.html'],
        js: ['Scripts/app/*.module.js','Scripts/app/*.js', 'Scripts/app/controllers/*.js', 'Scripts/app/services/*.js', 'Scripts/app/components/*.js']
    };

    var gruntConfig = {
        buildFolder: 'dist/build',
        baseDeployment: 'dist',
        deploymentFolder: 'dist/deploy',
        packagesFolder: 'packages',
        concatFiles: ['node_modules/jquery/dist/jquery.min.js',
            'node_modules/tether/dist/js/tether.min.js',
            'node_modules/bootstrap/dist/js/bootstrap.min.js',
            'node_modules/angular/angular.min.js',
            'node_modules/angular-ui-router/release/angular-ui-router.min.js',
            'node_modules/angular-animate/angular-animate.min.js',
            'node_modules/angular-touch/angular-touch.min.js',
            'node_modules/angular-ui-bootstrap/ui-bootstrap-tpls.min.js',
            'node_modules/angular-ui-grid/ui-grid.min.js',
            'packages/WK.Axcess.ConsumerLayer.analytics.1.0.2.0/content/scripts/analytics/*.all.min.js',
            'packages/WK.Axcess.ConsumerLayer.notifications.1.0.1.3/content/scripts/notifications/notifications.all.min.js',
            'packages/WK.Axcess.ConsumerLayer.AxcessHttp.1.0.5.0/content/scripts/axcesshttp/axcesshttp.all.min.js',
            'packages/WK.Axcess.ConsumerLayer.PercentFilter.1.0.1.1/content/scripts/percentfilter/percent.filter.all.min.js',
            'packages/WK.Axcess.ConsumerLayer.MessageBox.1.0.2.2/content/scripts/messagebox/message.all.min.js',
            'packages/WK.Axcess.ConsumerLayer.DarkFeatures.1.0.1.3/content/scripts/darkfeatures/darkfeatures.all.min.js',
            '<%= config.buildFolder %>/<%= pkg.name %>.min.js'],
         concatCSS: ['node_modules/bootstrap/dist/css/bootstrap.min.css',
            'node_modules/angular-ui-grid/ui-grid.min.css',
            'packages/WK.Axcess.ConsumerLayer.Notifications.1.0.1.3/content/scripts/notifications/angular-toastr.min.css',
            'css/theme.css',
            'css/site.css',]
    };

    module.exports = function (grunt) {

        // Load grunt tasks automatically
        require('load-grunt-tasks')(grunt);

        grunt.initConfig({
            config: gruntConfig,
            pkg: grunt.file.readJSON('package.json'),
            bootlint: {
                options: {
                    showallerrors: true,
                    relaxerror: [
                        // These errors can safely be ignored.
                        'W001', 'W002', 'W003', 'W005', 'W012', 'E013'
                    ]
                },
                files: fileLists.html
            },
            nugetrestore: {
                restore: {
                    src: 'packages.config',
                    dest: 'packages/',
                    options: {
                        configFile: 'nuget.config'
                    }
                }
            },
            jshint: {
                src: fileLists.js
            },
            htmlhint: {
                all: {
                    src: fileLists.html
                }
            },
            clean: {
                all: {
                    src: [gruntConfig.baseDeployment, gruntConfig.buildFolder, gruntConfig.packagesFolder]
                },
                after: {
                    src: [gruntConfig.buildFolder, gruntConfig.deploymentFolder]
                }
            },
            concat: {
                options: {
                    stripBanners: true,
                    banner: '/*! <%= pkg.name %> WK Build <%= grunt.template.today("yyyy-mm-dd") %> */\n'
                },
                dist: {
                    src: gruntConfig.concatFiles,
                    dest: '<%= config.buildFolder %>/<%= pkg.name %>.min.all.js'
                },
                css: {
                    src: gruntConfig.concatCSS,
                    dest: '<%- config.buildFolder %>/site.css'
                }
            },
            uglify: {
                options: {

                },
                build: {
                    src: fileLists.js,
                    dest: '<%= config.buildFolder %>/<%= pkg.name %>.min.js'
                }
            },
            copy: {
                deployContent: {
                    expand: true,
                    src: ['content/*.*', 'content/**/*.*', 'index.html', 'views/*.html', 'views/**/*.html', 'images/*.*'],
                    dest: gruntConfig.deploymentFolder
                },
                uigrid: {
                    expand: true,
                    cwd: 'node_modules/angular-ui-grid',
                    src: ['ui-grid.woff', 'ui-grid.ttf'],
                    dest: '<%= config.deploymentFolder %>/css'
                },
                CSS: {
                    expand: true,
                    cwd: '<%- config.buildFolder %>',
                    src: ['site.css'],
                    dest: '<%= config.deploymentFolder %>/css'
                },
                fonts: {
                    expand: true,
                    cwd: 'node_modules/bootstrap/dist',
                    src: ['fonts/*.*'],
                    dest: gruntConfig.deploymentFolder
                },
                deployJS: {
                    expand: true,
                    cwd: gruntConfig.buildFolder,
                    src: ['<%= pkg.name %>.min.all.js'],
                    dest: '<%= config.deploymentFolder %>/scripts'
                },
                darkFeatures: {
                    expand: true,
                    cwd: 'scripts/darkfeatures',
                    src: ['*.js'],
                    dest: '<%= config.deploymentFolder %>/scripts'
                }
            },
        });

        grunt.registerTask('dependencies', [
            'nuget'
        ]);

        grunt.registerTask('nuget',[
            'nugetrestore'
        ]);

        grunt.registerTask('validate', [
            'jshint',
            'htmlhint',
            'bootlint'
        ]);

        grunt.registerTask('build', [
            'uglify',
            'concat'
        ]);

        grunt.registerTask('deploy', [
            'copy'
        ]);

        grunt.registerTask('builddeploy', [
          'validate',
          'build',
          'deploy'
        ]);

        grunt.registerTask('default', [
            'clean:all',
            'dependencies',
            'validate',
            'build',
            'deploy'
        ]);

        grunt.registerTask('noclean', [
            'dependencies',
            'validate',
            'build',
            'deploy'
        ]);

    };
})();
