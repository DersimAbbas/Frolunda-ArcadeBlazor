window.setAuthToken = function (token) {
    document.cookie = `token=${token}; path=/; secure; samesite=strict`;
}