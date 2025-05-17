function initializeIndex(urls) {
    function loadPasswords() {
        $.get(urls.getPasswords, function (response) {
            if (response.success) {
                const tbody = $('#passwordsTable tbody');
                tbody.empty();

                response.data.forEach(function (item) {
                    var row = '<tr data-id="' + item.id + '">' +
                        '<td>' + item.siteName + '</td>' +
                        '<td>' + item.username + '</td>' +
                        '<td>' +
                        '<div class="input-group">' +
                        '<input type="password" class="form-control password-field" value="' + item.password + '" readonly>' +
                        '<button class="btn btn-outline-secondary toggle-password" type="button">' +
                        '<i class="bi bi-eye"></i>' +
                        '</button>' +
                        '<button class="btn btn-outline-secondary copy-password" type="button" title="Şifreyi Kopyala">' +
                        '<i class="bi bi-clipboard"></i>' +
                        '</button>' +
                        '</div>' +
                        '</td>' +
                        '<td>' +
                        '<button class="btn btn-sm btn-primary edit-password">Düzenle</button> ' +
                        '<button class="btn btn-sm btn-danger delete-password">Sil</button>' +
                        '</td>' +
                        '</tr>';
                    tbody.append(row);
                });
            } else {
                alert(response.message);
            }
        });
    }

    $(document).on('click', '.toggle-password', function () {
        const input = $(this).closest('td').find('input');
        const icon = $(this).find('i');
        
        if (input.attr('type') === 'password') {
            input.attr('type', 'text');
            icon.removeClass('bi-eye').addClass('bi-eye-slash');
        } else {
            input.attr('type', 'password');
            icon.removeClass('bi-eye-slash').addClass('bi-eye');
        }
    });

    $(document).on('click', '.copy-password', function() {
        const input = $(this).closest('td').find('input');
        const password = input.val();
        
        const tempInput = $('<input>');
        $('body').append(tempInput);
        tempInput.val(password).select();
        
        try {
            document.execCommand('copy');
            const icon = $(this).find('i');
            icon.removeClass('bi-clipboard').addClass('bi-clipboard-check');
            setTimeout(() => {
                icon.removeClass('bi-clipboard-check').addClass('bi-clipboard');
            }, 1500);
        } catch (err) {
            alert('Şifre kopyalanamadı!');
        }
        
        tempInput.remove();
    });

    $('#savePassword').on('click', function () {
        const data = {
            siteName: $('#siteName').val(),
            username: $('#siteUsername').val(),
            password: $('#sitePassword').val()
        };

        console.log('Gönderilen veri:', data);

        $.ajax({
            url: urls.addPassword,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(data),
            success: function (response) {
                if (response.success) {
                    $('#addPasswordModal').modal('hide');
                    $('#addPasswordForm')[0].reset();
                    loadPasswords();
                } else {
                    alert(response.message || 'Şifre eklenirken bir hata oluştu!');
                }
            },
            error: function (xhr, status, error) {
                console.error('XHR:', xhr);
                console.error('Status:', status);
                console.error('Error:', error);
                if (xhr.responseJSON && xhr.responseJSON.message) {
                    alert(xhr.responseJSON.message);
                } else {
                    alert('Şifre eklenirken bir hata oluştu! Lütfen tekrar giriş yapın ve deneyin.');
                }
            }
        });
    });

    $(document).on('click', '.edit-password', function () {
        const row = $(this).closest('tr');
        const id = row.data('id');
        const siteName = row.find('td:eq(0)').text();
        const username = row.find('td:eq(1)').text();
        const password = row.find('.password-field').val();

        $('#editId').val(id);
        $('#editSiteName').val(siteName);
        $('#editSiteUsername').val(username);
        $('#editSitePassword').val(password);

        $('#editPasswordModal').modal('show');
    });

    $('#updatePassword').on('click', function () {
        const data = {
            id: $('#editId').val(),
            siteName: $('#editSiteName').val(),
            username: $('#editSiteUsername').val(),
            password: $('#editSitePassword').val()
        };

        console.log('Güncelleme için gönderilen veri:', data);

        $.ajax({
            url: urls.updatePassword,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(data),
            success: function (response) {
                if (response.success) {
                    $('#editPasswordModal').modal('hide');
                    loadPasswords();
                } else {
                    alert(response.message || 'Şifre güncellenemedi!');
                }
            },
            error: function (xhr, status, error) {
                console.error('XHR:', xhr);
                console.error('Status:', status);
                console.error('Error:', error);
                if (xhr.responseJSON && xhr.responseJSON.message) {
                    alert(xhr.responseJSON.message);
                } else {
                    alert('Şifre güncellenirken bir hata oluştu!');
                }
            }
        });
    });

    $(document).on('click', '.delete-password', function () {
        if (confirm('Bu şifreyi silmek istediğinizden emin misiniz?')) {
            const id = $(this).closest('tr').data('id');
            
            $.ajax({
                url: urls.deletePassword,
                type: 'POST',
                data: { id: id },
                success: function (response) {
                    if (response.success) {
                        loadPasswords();
                    } else {
                        alert('Şifre silinemedi!');
                    }
                },
                error: function () {
                    alert('Bir hata oluştu!');
                }
            });
        }
    });

    function generateRandomPassword(options) {
        const uppercase = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ';
        const lowercase = 'abcdefghijklmnopqrstuvwxyz';
        const numbers = '0123456789';
        const symbols = '!@#$%^&*()_+-=[]{}|;:,.<>?';
        
        let chars = '';
        let password = '';
        
        if (options.useUppercase) chars += uppercase;
        if (options.useLowercase) chars += lowercase;
        if (options.useNumbers) chars += numbers;
        if (options.useSymbols) chars += symbols;
        
        if (chars === '') chars = lowercase;
        
        const length = Math.min(Math.max(options.length, 8), 32);
        
        for (let i = 0; i < length; i++) {
            const randomIndex = Math.floor(Math.random() * chars.length);
            password += chars[randomIndex];
        }
        
        return password;
    }
    
    $('#generatePassword').on('click', function() {
        const options = {
            useUppercase: $('#useUppercase').is(':checked'),
            useLowercase: $('#useLowercase').is(':checked'),
            useNumbers: $('#useNumbers').is(':checked'),
            useSymbols: $('#useSymbols').is(':checked'),
            length: parseInt($('#passwordLength').val())
        };
        
        const password = generateRandomPassword(options);
        $('#sitePassword').val(password);
    });

    loadPasswords();
} 