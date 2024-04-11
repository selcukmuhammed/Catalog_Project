$.ajax({
    url: '/api/Authentication/login',
    method: 'POST',
    contentType: 'application/json',
    data: JSON.stringify({ UserName: 'kullanıcıAdı', Password: 'şifre' }),
    success: function (data) {
        localStorage.setItem('token', data.token);
    },
    error: function (xhr, status, error) {
    }
});
