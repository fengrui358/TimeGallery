$(function () {
    var uploader = Qiniu.uploader({
        runtimes: 'html5,flash,html4', // 上传模式,依次退化
        browse_button: 'pickfiles', // 上传选择的点选按钮，**必需**
        // 在初始化时，uptoken, uptoken_url, uptoken_func 三个参数中必须有一个被设置
        // 切如果提供了多个，其优先级为 uptoken > uptoken_url > uptoken_func
        // 其中 uptoken 是直接提供上传凭证，uptoken_url 是提供了获取上传凭证的地址，如果需要定制获取 uptoken 的过程则可以设置 uptoken_func
        // uptoken : '<Your upload token>', // uptoken 是上传凭证，由其他程序生成
        uptoken_url: globalOptions.qiniu.uptoken_url, // Ajax 请求 uptoken 的 Url，**强烈建议设置**（服务端提供）
        // uptoken_func: function(file){    // 在需要获取 uptoken 时，该方法会被调用
        //    // do something
        //    return uptoken;
        // },
        get_new_uptoken: false, // 设置上传文件的时候是否每次都重新获取新的 uptoken
        // downtoken_url: '/downtoken',
        // Ajax请求downToken的Url，私有空间时使用,JS-SDK 将向该地址POST文件的key和domain,服务端返回的JSON必须包含`url`字段，`url`值为该文件的下载地址
        unique_names: true,              // 默认 false，key 为文件名。若开启该选项，JS-SDK 会为每个文件自动生成key（文件名）
        // save_key: true,                  // 默认 false。若在服务端生成 uptoken 的上传策略中指定了 `sava_key`，则开启，SDK在前端将不对key进行任何处理
        domain: globalOptions.qiniu.domain, // bucket 域名，下载资源时用到，**必需**
        container: 'cover_upload_container', // 上传区域 DOM ID，默认是 browser_button 的父元素        
        filters: {
            mime_types: [//只允许上传图片和zip文件
                { title: "Image files", extensions: "jpg,jpeg,gif,png" }
            ],
            max_file_size: globalOptions.qiniu.max_file_size, //最大文件体积限制
            prevent_duplicates: false //不允许选取重复文件
        },
        flash_swf_url: 'path/of/plupload/Moxie.swf', //引入 flash,相对路径
        max_retries: 3, // 上传失败最大重试次数
        dragdrop: true, // 开启可拖曳上传
        drop_element: 'cover_upload_container', // 拖曳上传区域元素的 ID，拖曳文件或文件夹后可触发上传
        chunk_size: '4mb', // 分块上传时，每块的体积
        auto_start: false, // 选择文件后自动上传，若关闭需要自己绑定事件触发上传,
        multi_selection: false, //仅允许单选
        //x_vars : {
        //    自定义变量，参考http://developer.qiniu.com/docs/v6/api/overview/up/response/vars.html
        //    'time' : function(up,file) {
        //        var time = (new Date()).getTime();
        // do something with 'time'
        //        return time;
        //    },
        //    'size' : function(up,file) {
        //        var size = file.size;
        // do something with 'size'
        //        return size;
        //    }
        //},        
        is_selected_cover: false,
        init: {
            'FilesAdded': function (up, files) {
                if (files.length > 0) {
                    try {
                        var file = files[0].getNative();
                        var reader = new FileReader();
                        if (/image\/\w+/.test(file.type)) {
                            reader.onload = function () {
                                //todo:修改上传图片的显示
                                $('#galleryCoverImg').attr('src', this.result);
                            }

                            reader.readAsDataURL(file);

                            uploader.is_selected_cover = true;
                        }
                    } catch (e) {
                        console.debug(e);
                    }
                }
            },
            'BeforeUpload': function(up, file) {
                // 每个文件上传前,处理相关的事情
                
            },
            'UploadProgress': function(up, file) {
                // 每个文件上传时,处理相关的事情
                
            },
            'FileUploaded': function(up, file, info) {
                // 每个文件上传成功后,处理相关的事情
                // 其中 info 是文件上传成功后，服务端返回的json，形式如
                // {
                //    "hash": "Fh8xVqod2MQ1mocfI4S4KpRL6D98",
                //    "key": "gogopher.jpg"
                //  }
                // 参考http://developer.qiniu.com/docs/v6/api/overview/up/response/simple-response.html

                // var domain = up.getOption('domain');
                // var res = parseJSON(info);
                // var sourceLink = domain + res.key; 获取上传成功后的文件的Url

                var domain = up.getOption('domain');
                var res = $.parseJSON(info);
                var sourceLink = domain + res.key;

                $.post("RegisterSubmit",
                    { name: $('#galleryName').val(), description: $('#galleryDescription').val(), coverUrl: sourceLink },
                    function(data) {
                        $('#loadingToast').hide();

                        if (data.state !== 0) {
                            $.showWarning(data.message);
                            return;
                        }

                        if (data.state === 0) {
                            $('.weui_msg .weui_msg_desc').html(data.message);

                            $('#registerContainer').hide();
                            $('.weui_msg').show();
                        }
                    },
                    "json").error(function() {
                    alert('服务端异常');
                });
            },
            'Error': function(up, err, errTip) {
                //上传出错时,处理相关的事情
                //todo:完善错误提示
            },
            'UploadComplete': function() {
                //队列文件处理完毕后,处理相关的事情
            },
            'Key': function(up, file) {
                // 若想在前端对每个文件的key进行个性化处理，可以配置该函数
                // 该配置必须要在 unique_names: false , save_key: false 时才生效

                var key = "";
                // do something with key here
                return key;
            }
        }
    });
    // domain 为七牛空间（bucket)对应的域名，选择某个空间后，可通过"空间设置->基本设置->域名设置"查看获取
    // uploader 为一个 plupload 对象，继承了所有 plupload 的方法，参考http://plupload.com/docs

    $('#registerSubmit')
        .on('click',
            function() {
                var galleryName = $('#galleryName').val();
                if (galleryName == null || galleryName === '') {
                    $.showWarning("相册名不能为空");
                    return;
                }

                $('#loadingToast').show();

                if (uploader.is_selected_cover) {
                    uploader.start();
                } else {
                    $.post("RegisterSubmit",
                        { name: galleryName, description: $('#galleryDescription').val() },
                        function (data) {
                            $('#loadingToast').hide();

                            if (data.state !== 0) {
                                $.showWarning(data.message);
                                return;
                            }

                            if (data.state === 0) {
                                $('.weui_msg .weui_msg_desc').html(data.message);

                                $('#registerContainer').hide();
                                $('.weui_msg').show();
                            }
                        },
                        "json");
                }
            });
});