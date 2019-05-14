// HARD CODED TEST API RESPONSE
/*
var data = { "host": "https:\/\/dev-api.quebon.tv", "userid": "1662593299120151", "nickname": "", "token": "eyJhbGciOiJIUzI1NiJ9.eyJleHAiOjE1NTc3NDQ3NTAsInR5cGUiOiJJTkRWIiwiaWQiOiIxNjYyNjgzNzM2NzAzMDExIiwic2Vzc2lvbklkIjoiMmZhZjdhY2QtMWYxNy00OGZkLTgyODYtNTEyNDE2ZThiMzNhIiwiYXV0aExldmVsIjoxLCJyb2xlcyI6W10sInN1YnNjcmlwdGlvbiI6eyJzdWJzY3JpcHRpb25JZCI6IjE2NjI3NDMzNTU1NzUzMzQiLCJlbmREYXRlIjoiMjAxOS0wMS0xNiIsImFjdGl2ZSI6ZmFsc2V9LCJyZWFkT25seSI6ZmFsc2UsImlhdCI6MTU1NzcyMzE1MH0.YwAs5Z4P7PNf_EZpV7t9t7wwyvs7eBn3q-SnRERcBE0", "closeUrl": "https:\/\/dev.quebon.tv\/game\/toRect\/exit" };

// this function requests with user id/pw, and recieves user info and security token.
var SetUserData = function () {
    var http = new XMLHttpRequest();
    var url = 'https://dev-api.quebon.tv/user/v1/users/generateAuthToken';
    var data = null;
    http.open('POST', url, false);
    http.onreadystatechange = function () {
        if (http.readyState == 4 && http.status == 200) {
            data = http.responseText; // keys : host, userid, nickname, token, closeUrl
            if (data == null) console.log("error, data returns null");
        }
    }
    gameInstance.SendMessage("EC", "SetUserData", JSON.stringify(data));
};
*/