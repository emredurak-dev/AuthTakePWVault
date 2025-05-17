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

    // Şifre göster/gizle
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

    // Yeni şifre ekleme
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

    // Şifre düzenleme modalını aç
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

    // Şifre güncelleme
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

    // Şifre silme
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

    // Sayfa yüklendiğinde şifreleri yükle
    loadPasswords();
} 