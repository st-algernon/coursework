/// <reference types="cypress" />

describe('test home page', () => {
  beforeEach(() => {
    cy.visit('/');
  });

  it('should see more the largest collection', () => {
    cy.get('[data-cy="collection"]', { timeout: 10000 }).should(
      'have.length',
      2
    );
    cy.get('[data-cy="see-more-collections-btn"]').click();
    cy.get('[data-cy="collection"]').should('have.length', 2);
  });

  it('should find items by click tag', () => {
    cy.get('[data-cy="top-tags-container"]', { timeout: 10000 })
      .get('[data-cy="top-tags-item"]')
      .eq(0)
      .click();
    cy.location('pathname').should('include', 'search');
  });
});
