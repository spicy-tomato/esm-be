package com.example.ems.user.entity;

import com.fasterxml.jackson.annotation.JsonCreator;
import com.fasterxml.jackson.annotation.JsonIgnore;
import com.fasterxml.jackson.annotation.JsonProperty;
import lombok.Builder;
import lombok.Value;
import org.springframework.security.core.GrantedAuthority;
import org.springframework.security.core.userdetails.UserDetails;

import java.io.Serial;
import java.util.ArrayList;
import java.util.Collection;
import java.util.Objects;

@Value
@Builder
public
class User implements UserDetails {
    @Serial
    private static final long serialVersionUID = -6425700389277434388L;

    String id;
    String username;
    String password;

    @JsonCreator
    User(@JsonProperty("id") final String id,
         @JsonProperty("username") final String username,
         @JsonProperty("password") final String password) {
        super();
        this.id = Objects.requireNonNull(id);
        this.username = Objects.requireNonNull(username);
        this.password = Objects.requireNonNull(password);
    }

    @JsonIgnore
    @Override
    public
    Collection<GrantedAuthority> getAuthorities() {
        return new ArrayList<>();
    }

    @JsonIgnore
    @Override
    public
    boolean isAccountNonExpired() {
        return true;
    }

    @JsonIgnore
    @Override
    public
    boolean isAccountNonLocked() {
        return true;
    }

    @JsonIgnore
    @Override
    public
    boolean isCredentialsNonExpired() {
        return true;
    }

    @Override
    public
    boolean isEnabled() {
        return true;
    }

    @JsonIgnore
    public
    String getPassword() {
        return password;
    }
}
