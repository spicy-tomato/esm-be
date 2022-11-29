package com.example.ems.user.controller;

import com.example.ems.auth.api.IUserAuthenticationService;
import com.example.ems.user.crud.api.IUserCrudService;
import com.example.ems.user.entity.User;
import lombok.AccessLevel;
import lombok.AllArgsConstructor;
import lombok.NonNull;
import lombok.experimental.FieldDefaults;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequestMapping("/public/users")
@FieldDefaults(level=AccessLevel.PRIVATE, makeFinal=true)
@AllArgsConstructor(access=AccessLevel.PACKAGE)
final
class PublicUsersController {
    @NonNull
    IUserAuthenticationService authentication;
    @NonNull
    IUserCrudService users;

    @PostMapping("register")
    String register(@RequestParam("username") final String username, @RequestParam("password") final String password) {
        users.save(User.builder()
                       .id(username)
                       .username(username)
                       .password(password)
                       .build());

        return login(username, password);
    }

    @PostMapping("/login")
    String login(@RequestParam("username") final String username, @RequestParam("password") final String password) {
        return authentication.login(username, password)
            .orElseThrow(() -> new RuntimeException("Invalid username or password"));
    }
}
