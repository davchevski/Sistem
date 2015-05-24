function ShowValidationSuccess(successMsg) {
    toastr.options = {
        positionClass: 'toast-bottom-left',
        onclick: null,
        fadeIn: '300',
        fadeOut: '5000',
        timeOut: '7000'
    },
    toastr.success(successMsg);
}

function ShowValidationFailure(errorMsg) {
    toastr.options = {
        positionClass: 'toast-bottom-left',
        onclick: null,
        fadeIn: '300',
        fadeOut: '5000',
        timeOut: '7000'
    },
    toastr.error(errorMsg);
}