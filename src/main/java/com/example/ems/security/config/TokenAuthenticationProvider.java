package com.example.ems.security.config;

import com.example.ems.user.service.auth.UserAuthenticationService;
import lombok.AccessLevel;
import lombok.AllArgsConstructor;
import lombok.NonNull;
import lombok.experimental.FieldDefaults;
import org.springframework.security.authentication.UsernamePasswordAuthenticationToken;
import org.springframework.security.authentication.dao.AbstractUserDetailsAuthenticationProvider;
import org.springframework.security.core.AuthenticationException;
import org.springframework.security.core.userdetails.UserDetails;
import org.springframework.security.core.userdetails.UsernameNotFoundException;
import org.springframework.stereotype.Component;

import java.util.Optional;

@Component
@AllArgsConstructor(access=AccessLevel.PACKAGE)
@FieldDefaults(level=AccessLevel.PRIVATE, makeFinal=true)
final
class TokenAuthenticationProvider extends AbstractUserDetailsAuthenticationProvider {
    @NonNull
    UserAuthenticationService auth;

    @Override
    protected
    void additionalAuthenticationChecks(final UserDetails userDetails, final UsernamePasswordAuthenticationToken auth)
    throws AuthenticationException {}

    @Override
    protected
    UserDetails retrieveUser(final String username, final UsernamePasswordAuthenticationToken authentication)
    throws AuthenticationException {
        final Object token = authentication.getCredentials();
        System.out.println("retrieveUser");
        return Optional.ofNullable(token)
            .map(String::valueOf)
            .flatMap(auth::findByToken)
            .orElseThrow(() -> new UsernameNotFoundException(
                "Cannot find user with authentication token=" + token));
    }
}
