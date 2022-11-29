package com.example.ems.user.service.auth;

import com.example.ems.user.entity.User;

import java.util.Optional;

public
interface UserAuthenticationService {
    Optional<String> login(String username, String password);

    Optional<User> findByToken(String token);

    void logout(User user);
}
