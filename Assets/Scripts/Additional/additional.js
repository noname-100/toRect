// HARD CODED TEST API RESPONSE
// var data = {"host":"https:\/\/dev-api.quebon.tv","userid":"1662593299120151","nickname":"","token":"eyJhbGciOiJIUzI1NiJ9.eyJleHAiOjE1NTc0OTg2NzMsInR5cGUiOiJJTkRWIiwiaWQiOiIxNjYyNTkzMjk5MTIwMTUxIiwic2Vzc2lvbklkIjoiNjNmZjk1ZmEtZGQxNy00MzhiLWI1M2QtYjQ3Y2ZiZDA0MDUzIiwiYXV0aExldmVsIjoxLCJyb2xlcyI6W10sInN1YnNjcmlwdGlvbiI6eyJzdWJzY3JpcHRpb25JZCI6IjIyMjczMjkyNDYwNTk1MjIiLCJlbmREYXRlIjoiMjAxOS0wNy0wMiIsImFjdGl2ZSI6dHJ1ZX0sInJlYWRPbmx5IjpmYWxzZSwiaWF0IjoxNTU3NDc3MDczfQ.Xp-D3xJ3xIEGso8KKwip0jPzI2zfsgCju8hV2NmQIdA","closeUrl":"https:\/\/dev.quebon.tv\/game\/toRect\/exit"};

// this function requests with user id/pw, and recieves user info and security token.
var SetUserData = function() {
     var http = new XMLHttpRequest();
     var url = 'https://dev-api.quebon.tv/user/v1/users/generateAuthToken';
     var data = null;
     http.open('POST', url, false);
     http.onreadystatechange = function(){
         if(http.readyState == 4 && http.status == 200){
             data = http.responseText; // keys : host, userid, nickname, token, closeUrl
             if(data == null) console.log("error, data returns null");
         }
     }
     gameInstance.SendMessage("EC", "SetUserData", JSON.stringify(data));
}