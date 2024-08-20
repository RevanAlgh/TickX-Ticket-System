export const Constant = {
  // We will store validation messages, API end point
  API_END_POINT: {
    GET_CLIENTS: 'Client/clients/',
    GET_CLIENT: 'Client/client/',
    GET_EMPLOYEE: 'TeamMember/team-member/',
    GET_EMPLOYEES: 'TeamMember/team-members/',
    TOGGLE_ACTIVATION: 'TeamMember/team-member/toggle-activation/',
    CLIENT_TOGGLE_ACTIVATION: 'Client/client/toggle-activation/',
    ACTIVE_EMPLOYEES: 'TeamMember/active-team-members/',
    ADD_EMPLOYEE: 'TeamMember/team-member/',
    UPDATE_CLIENT: 'Client/client/',
    USER_LOGIN: 'User/login',
    LOAD_RECENT_TICKETS: 'Dashboard/recent-tickets/',
    CREATE_TICKET: '',
    USER_REGISTER: 'User/register',
    ADD_TICKET: 'Ticket/ticket',
    GET_TICKETS: 'Ticket/tickets-by-client/',
    GET_PRODUCTS: 'Product',
    GET_ALL_TICKETS: 'Ticket/tickets',
    UPDATE_TICKET: 'Ticket/ticket/edit',
    ASSIGN_TICKET: 'Ticket/assign-ticket',
    ASSIGN_PRIORITY: 'Ticket/ticket/priority',
    GET_ASSIGN_TO_TICKETS: 'Ticket/assignedTo/',
    GET_TICKETS_BY_PRIORITY: 'Ticket/tickets-by-priority/',
    GET_TICKETS_BY_STATUS: 'Ticket/tickets-by-status/',
    GET_COMMENTS: 'Comment/get-comments/',
    ADD_COMMENT: 'Comment/add-comment/',
    UPDATE_STATUS: '',
    SET_TICKET_STATUS: 'Ticket/ticket/status/',
    GET_FILTERED_TICKETS: 'Ticket/filter-tickets/',
    ASK_GPT: 'ChatGPT/ask/',
    ADD_PRODUCT: 'Product',

    USER_TICKETS_STATUS_COUNT: 'Dashboard/status-count/',
    USER_TICKET_PRIORITY_COUNT: 'Dashboard/priority-count/',
    GET_TICKETS_OF_YEAR_GROUP_BY_MONTHS:
      'ManagerDashboard/tickets-current-year',
    GET_TICKETS_GROUP_BY_PRIORITY: 'ManagerDashboard/tickets-by-priority/',

    GET_MANGER_TICKETS_PER_MONTH: 'ManagerDashboard/tickets-per-month', //taked year and month
    GET_MANGER_TICKETS_PER_YEAR: 'ManagerDashboard/tickets-per-year', //only year
    TOTAL_TICKETS_SUMMARY: 'ManagerDashboard/total-tickets-summary', // TAKES status and priority
    GET_TOP_EMPLOYEES: 'ManagerDashboard/top-employees',
    GET_TOTAL_TICKETS: 'Dashboard/total-tickets',
    GET_STAT: 'ManagerDashboard/statistics',

    GET_TICKETS_BY_STATUS_MANGER: 'ManagerDashboard/tickets-by-status',
    GET_TICKETS_USER_PER_MONTH: 'Dashboard/tickets-by-month',

    USER_GET_TICKETS_BY_PRIORITY: 'Dashboard/total-tickets-by-priority',
    GET_TICKETS_USER_PER_YEAR: 'Dashboard/tickets-by-year',

    RESET_PASSWORD: 'User/request-reset',

    RESET_PASSWORD_WITH_URL: 'User/reset',
  },
  VALIDATION_MESSAGE: {
    REQUIRED: 'This Is Required',
  },
};
