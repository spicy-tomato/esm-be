package com.example.ems.security.config;

import jakarta.servlet.http.HttpServletRequest;
import jakarta.servlet.http.HttpServletResponse;
import org.springframework.security.web.RedirectStrategy;

class NoRedirectStrategy implements RedirectStrategy {
    @Override
    public
    void sendRedirect(final HttpServletRequest request, final HttpServletResponse response, final String url) {}
}
