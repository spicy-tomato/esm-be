package com.example.ems.security.config;

import jakarta.servlet.FilterChain;
import jakarta.servlet.ServletException;
import jakarta.servlet.http.HttpServletRequest;
import jakarta.servlet.http.HttpServletResponse;
import lombok.AccessLevel;
import lombok.experimental.FieldDefaults;
import org.springframework.security.authentication.BadCredentialsException;
import org.springframework.security.authentication.UsernamePasswordAuthenticationToken;
import org.springframework.security.core.Authentication;
import org.springframework.security.core.AuthenticationException;
import org.springframework.security.web.authentication.AbstractAuthenticationProcessingFilter;
import org.springframework.security.web.util.matcher.RequestMatcher;
import org.apache.commons.lang3.StringUtils;

import java.io.IOException;
import java.util.Optional;

import static org.springframework.http.HttpHeaders.AUTHORIZATION;

@FieldDefaults(level=AccessLevel.PRIVATE, makeFinal=true)
final
class TokenAuthenticationFilter extends AbstractAuthenticationProcessingFilter {
    private static final String BEARER = "Bearer";

    TokenAuthenticationFilter(final RequestMatcher requiresAuth) {
        super(requiresAuth);
    }

    @Override
    public
    Authentication attemptAuthentication(final HttpServletRequest request, final HttpServletResponse response)
    throws AuthenticationException {
        final String param = Optional.ofNullable(request.getHeader(AUTHORIZATION))
            .orElse(request.getParameter("t"));
        final String token = Optional.ofNullable(param)
            .map(value -> StringUtils.removeStart(value, BEARER))
            .map(String::trim)
            .orElseThrow(() -> new BadCredentialsException("Missing authentication token"));
        final Authentication auth = new UsernamePasswordAuthenticationToken(token, token);
        return getAuthenticationManager().authenticate(auth);
    }

    @Override
    protected
    void successfulAuthentication(final HttpServletRequest request,
                                  final HttpServletResponse response,
                                  final FilterChain chain,
                                  final Authentication authResult) throws IOException, ServletException {
        super.successfulAuthentication(request, response, chain, authResult);
        chain.doFilter(request, response);
    }
}
