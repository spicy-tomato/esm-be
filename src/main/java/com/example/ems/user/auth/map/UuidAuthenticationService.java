package com.example.ems.user.auth.map;

import com.example.ems.auth.api.IUserAuthenticationService;
import com.example.ems.user.crud.api.IUserCrudService;
import com.example.ems.user.entity.User;
import lombok.AccessLevel;
import lombok.AllArgsConstructor;
import lombok.NonNull;
import lombok.experimental.FieldDefaults;
import org.springframework.stereotype.Service;

import java.util.Optional;
import java.util.UUID;

@Service
@AllArgsConstructor(access=AccessLevel.PACKAGE)
@FieldDefaults(level=AccessLevel.PRIVATE, makeFinal=true)
final
class UuidAuthenticationService implements IUserAuthenticationService {
    @NonNull
    IUserCrudService users;

    @Override
    public
    Optional<String> login(final String username, final String password) {
        final String uuid = UUID.randomUUID()
            .toString();
        final User user = User.builder()
            .id(uuid)
            .username(username)
            .password(password)
            .build();

        users.save(user);
        return Optional.of(uuid);
    }

    @Override
    public
    Optional<User> findByToken(final String token) {
        return users.find(token);
    }

    @Override
    public
    void logout(final User user) {}
}
