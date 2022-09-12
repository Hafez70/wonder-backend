import React, { Component, useEffect, useState } from 'react';

var UserProfile = (function () {
    var userInfo = {};

    var getUserInfo = function () {
        return JSON.parse(localStorage.get("ls"));
        // Or pull this from cookie/localStorage
    };

    var setUserInfo = function (name) {
        return localStorage.setItem("ls", JSON.stringify(userInfo));
    };

    return {
        getUserInfo: getUserInfo,
        setUserInfo: setUserInfo
    }

})();

export default UserProfile;
