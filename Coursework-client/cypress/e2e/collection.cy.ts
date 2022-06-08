/// <reference types="cypress" />

describe('my collections test', () => {
  it('should add collection', () => {
    cy.visit('/');
    cy.fixture('auth-data').then((data) => {
      cy.login(data.email, data.password);
    });

    cy.get('.account-menu').click();
    cy.get('[data-cy="my-collections-btn"]').click();
    cy.location('pathname').should('include', '/collections');

    cy.get('[data-cy="add-collection-btn"]').click();
  });
});
