package com.example.ems.auth.api;

import com.example.ems.user.entity.User;

import java.util.Optional;

public
interface IUserAuthenticationService {
    Optional<String> login(String username, String password);

    Optional<User> findByToken(String token);

    void logout(User user);
}
