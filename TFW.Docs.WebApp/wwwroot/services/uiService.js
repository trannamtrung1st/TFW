function UiService({
    defaultConfirmTitle,
    defaultConfirmBtnText,
}) {
    return {
        changeLoading: (show) => {
            console.log('Loading', show);
        },

        showSuccess: ({
            message
        }) => {

        },

        showConfirm: ({
            heightAuto = false,
            title = defaultConfirmTitle,
            text,
            icon = 'warning',
            showCancelButton = true,
            confirmButtonColor = '#3085d6',
            cancelButtonColor = '#d33',
            confirmButtonText = defaultConfirmBtnText
        }) => {
            return Swal.fire({
                heightAuto,
                title,
                text,
                icon,
                showCancelButton,
                confirmButtonColor,
                cancelButtonColor,
                confirmButtonText
            });
        }
    };
}