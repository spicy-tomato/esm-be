package com.example.ems.user.crud.api;

import com.example.ems.user.entity.User;

import java.util.Optional;

public
interface IUserCrudService {
    User save(User user);

    Optional<User> find(String id);

    Optional<User> findByUsername(String username);
}
