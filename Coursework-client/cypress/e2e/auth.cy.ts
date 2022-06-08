/// <reference types="cypress" />

interface User {
  name: string;
  email: string;
  password: string;
}

let user: User;

describe('auth tests', () => {
  before(() => {
    cy.fixture('auth-data').then((data) => {
      this.user = data;
    });
  });

  beforeEach(() => {
    cy.visit('/');
    cy.viewport(1920, 1080);
  });

  it('should register', () => {
    cy.get('.footer__lang-select').click();
    cy.contains('en').click();
    cy.get('.header__login').click();
    cy.contains('Register').click();
    cy.get('input[type=text]').type(name);
    cy.get('input[type=email]').type(email);
    cy.get('input[type=password]').type(password).type('{enter}');

    cy.location('pathname').should('not.include', '/auth');
  });

  it('should login', () => {
    cy.login(user.email, user.password);
  });

  it('should logout', () => {
    cy.login(user.email, user.password);
    cy.get('.account-menu', { timeout: 30000 }).click();
    cy.get('[data-cy="logout-btn"]').click();
  });
});
